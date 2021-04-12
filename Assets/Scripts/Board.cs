using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] fruitsObjects;
    public GameObject[,] fruits;
    // Start is called before the first frame update
    void Start()
    {
        fruits = new GameObject[width, height];
        setUpBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setUpBoard(){
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                Vector2 instantiatePosition = new Vector2(i,j);
                GameObject tile = Instantiate(tilePrefab, instantiatePosition, Quaternion.identity);
                tile.transform.parent = this.transform;
                tile.transform.name = "(" + i + "," + j + ")";
                int randomFruits = Random.Range(0, fruitsObjects.Length);
                int whileIteration = 0;
                while(checkMatch(i,j, fruitsObjects[randomFruits].GetComponent<fruits>()) && whileIteration < 100){
                    randomFruits = Random.Range(0, fruitsObjects.Length);
                    whileIteration++;
                }
                GameObject fruit = Instantiate(fruitsObjects[randomFruits], instantiatePosition, Quaternion.identity);
                fruit.transform.parent = this.transform;
                fruit.transform.name = "(" + i + "," + j + ")";
                fruits[i,j] = fruit;
            }
        }
    }

    private bool checkMatch(int column, int row, fruits fruit){
        if(column > 1 && row > 1){
            if(fruits[column-1, row].GetComponent<fruits>().fruitName == fruit.fruitName && fruits[column-2, row].GetComponent<fruits>().fruitName == fruit.fruitName){
                return true;
            }
            if(fruits[column, row-1].GetComponent<fruits>().fruitName == fruit.fruitName && fruits[column, row-1].GetComponent<fruits>().fruitName == fruit.fruitName){
                return true;
            }
        } else{
            if(row > 1){
                if(fruits[column, row-1].GetComponent<fruits>().fruitName == fruit.fruitName && fruits[column, row-1].GetComponent<fruits>().fruitName == fruit.fruitName){
                    return true;
                }
            }
            if(column > 1){
                if(fruits[column-1, row].GetComponent<fruits>().fruitName == fruit.fruitName && fruits[column-2, row].GetComponent<fruits>().fruitName == fruit.fruitName){
                    return true;
                }
            }
        }
        return false;
    }
}
