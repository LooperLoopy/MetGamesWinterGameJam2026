using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onStart();
    }

    async void onStart(){
        await Task.Delay(1000);
        Destroy(this.gameObject);
    }
}
