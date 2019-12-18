using Holoville.HOTween;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.View.Character
{
    public sealed class DreamingBaloon : EventView
    {
        [SerializeField] private string _showAnimName = "show";
        [SerializeField] private string _hideAnimName = "hide";
        [SerializeField] private float _pictureAnimDuration = 0.3f;
		
		
        public tk2dSprite Picture;

        private Vector3 _normalPictureScale;

        private LWFAnimation _lwf;
		
        protected override void Start()
        {
            base.Start();

            Picture.gameObject.SetActive(false);
            _normalPictureScale = Picture.scale;
        }

        private void LWFAnimationHasBeenInited(LWFAnimation lwf)
        {
            _lwf = lwf;
            _lwf.Stop();
        }

        public void Show()
        {
            _lwf.OnPlayFinished = OnShowFinished;
            _lwf.Play(_showAnimName);
        }

        public void Hide()
        {
            var param = new TweenParms();
            param.Prop("scale", Vector3.zero);
            param.OnComplete(() => { _lwf.Play(_hideAnimName); Picture.gameObject.SetActive(false); });

            HOTween.To(Picture, _pictureAnimDuration, param);

        }

        private void OnShowFinished()
        {
            _lwf.OnPlayFinished = null;

            Picture.gameObject.SetActive(true);
            Picture.scale = Vector3.zero;

            HOTween.To(Picture, _pictureAnimDuration, "scale", _normalPictureScale);
        }
    }
}