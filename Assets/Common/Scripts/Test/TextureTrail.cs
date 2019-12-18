using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TextureTrail : MonoBehaviour
{
    [SerializeField] private GameObject _spriteTrailHead;

    [SerializeField]
    private List<GameObject> _spriteTrail;

    private bool _isHeadSpawned = false;

    private int _currentTrailIndex = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public TextureTrailItem SpawnNext(Transform point)
    {
        object obj;

        if(!_isHeadSpawned)
        {
            obj = Instantiate(_spriteTrailHead.transform, point.position, Quaternion.identity);
            _isHeadSpawned = true;
        }
        else
        {
            if (_currentTrailIndex == _spriteTrail.Count) _currentTrailIndex = 0;
            obj = Instantiate(_spriteTrail[_currentTrailIndex++].transform, point.position, Quaternion.identity);
        }


        return ((GameObject) obj).GetComponent<TextureTrailItem>();
    }

}
