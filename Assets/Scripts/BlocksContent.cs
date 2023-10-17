using UnityEngine;

public class BlocksContent : MonoBehaviour
{
    [SerializeField] private Points _points;
    [SerializeField] private AutoClick _autoClick;

    private Factory _factory;    

    private void Awake()
    {
        _factory = new Factory(_points, _autoClick);        

        _factory.InstantiateBloks(gameObject);
    }
}
