using System.Collections;
using Holoville.HOTween;
using UnityEngine;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public sealed class FootTrailItem : PoolObject
    {
        private tk2dSprite _sprite;
        private float _duration;

        public void Init(float duration)
        {
            _duration = duration;

            StartCoroutine(WaitAndDespawn());
        }

        protected override void OnPoolInitialize()
        {
            _sprite = GetComponent<tk2dSprite>();
        }
        
        private void Update()
        {
            _sprite.color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, (_duration - Age) / _duration);
        }

        private IEnumerator WaitAndDespawn()
        {
            yield return new WaitForSeconds(_duration);
            
            Despawn();
        }
        
    }
}