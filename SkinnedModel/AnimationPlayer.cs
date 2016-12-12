#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// The animation player is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields


        // Information about the currently playing animation clip.
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        int currentKeyframe;


        // Current animation transform matrices.
        readonly Matrix[] boneTransforms;
        readonly Matrix[] worldTransforms;
        readonly Matrix[] skinTransforms;


        // Backlink to the bind pose and skeleton hierarchy data.
        readonly SkinningData skinningDataValue;


        #endregion


        /// <summary>
        /// Constructs a new animation player.
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
            {
                throw new ArgumentNullException(nameof(skinningData));
            }

            this.skinningDataValue = skinningData;

            this.boneTransforms = new Matrix[skinningData.BindPose.Count];
            this.worldTransforms = new Matrix[skinningData.BindPose.Count];
            this.skinTransforms = new Matrix[skinningData.BindPose.Count];
        }


        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip(AnimationClip clip)
        {
            if (clip == null)
            {
                throw new ArgumentNullException(nameof(clip));
            }

            this.currentClipValue = clip;
            this.currentTimeValue = TimeSpan.Zero;
            this.currentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            this.skinningDataValue.BindPose.CopyTo(this.boneTransforms, 0);
        }

        public void ChangeClip(string clipName)
        {
            AnimationClip clip = this.skinningDataValue.AnimationClips[clipName];

            if (clip == null)
            {
                throw new ArgumentNullException($"Animation has no clip named {clipName}");
            }

            this.StartClip(clip);
        }

        /// <summary>
        /// Advances the current animation position.
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            this.UpdateBoneTransforms(time, relativeToCurrentTime);
            this.UpdateWorldTransforms(rootTransform);
            this.UpdateSkinTransforms();
        }


        /// <summary>
        /// Helper used by the Update method to refresh the BoneTransforms data.
        /// </summary>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (this.currentClipValue == null)
            {
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");
            }
            
            // Update the animation position.
            if (relativeToCurrentTime)
            {
                time += this.currentTimeValue;

                // If we reached the end, loop back to the start.
                while (time >= this.currentClipValue.Duration)
                {
                    time -= this.currentClipValue.Duration;
                }
            }

            if ((time < TimeSpan.Zero) || (time >= this.currentClipValue.Duration))
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }

            // If the position moved backwards, reset the keyframe index.
            if (time < this.currentTimeValue)
            {
                this.currentKeyframe = 0;
                this.skinningDataValue.BindPose.CopyTo(this.boneTransforms, 0);
            }

            this.currentTimeValue = time;

            // Read keyframe matrices.
            IList<Keyframe> keyframes = this.currentClipValue.Keyframes;

            while (this.currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[this.currentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > this.currentTimeValue)
                {
                    break;
                }

                // Use this keyframe.
                this.boneTransforms[keyframe.Bone] = keyframe.Transform;

                this.currentKeyframe++;
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            this.worldTransforms[0] = this.boneTransforms[0] * rootTransform;

            // Child bones.
            for (var bone = 1; bone < this.worldTransforms.Length; bone++)
            {
                var parentBone = this.skinningDataValue.SkeletonHierarchy[bone];

                this.worldTransforms[bone] = this.boneTransforms[bone] * this.worldTransforms[parentBone];
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (var bone = 0; bone < this.skinTransforms.Length; bone++)
            {
                this.skinTransforms[bone] = this.skinningDataValue.InverseBindPose[bone] * this.worldTransforms[bone];
            }
        }


        /// <summary>
        /// Gets the current bone transform matrices, relative to their parent bones.
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return this.boneTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices, in absolute format.
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return this.worldTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices,
        /// relative to the skinning bind pose.
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return this.skinTransforms;
        }


        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public AnimationClip CurrentClip => this.currentClipValue;


        /// <summary>
        /// Gets the current play position.
        /// </summary>
        public TimeSpan CurrentTime => currentTimeValue;
    }
}
