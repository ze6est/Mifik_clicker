using UnityEngine;

public class BlocksContent : MonoBehaviour
{
    [SerializeField] private Points _points;    

    private Factory _factory;    

    private void Awake()
    {
        _factory = new Factory(_points);        

        _factory.InstantiateBloks(gameObject);
    }
}
