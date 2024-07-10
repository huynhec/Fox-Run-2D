using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsGenerator : MonoBehaviour
{
    public ObjectPooler gemPooler;

    public void SpawnGems(Vector3 position, float groundWidth){
        int random = Random.Range(1,100);
        if(random <50){
            return;
        }
        int numberOfGems = (int) Random.Range(3f, groundWidth);
        float y = Random.Range(2,4);
        //  numberOfGems = numberOfGems - 5;
        for(int i =0; i<numberOfGems; i++){
            GameObject gem = gemPooler.GetPooledGameObject();
            
            float x = position.x - (groundWidth/2) + 1;

            gem.transform.position = new Vector3(
                x + i,
                position.y - y + 1,
                0
            );
            gem.SetActive(true);
        }
        
    }
    

}
