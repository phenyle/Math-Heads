using UnityEngine;

public struct transformMatrix
{
    public int[] values;
}

public class DatabasePuzzle02 : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;
    
    public transformMatrix[] transforms;

    transformMatrix t1, t2, t3, t4, t5, t6;

    public int[,] tragetMatracies = new int[,] { {15, 15}, {-21, 24}, {-30 ,51}, {-7, 72}, {9, 47}, {33, 35} }; 

    public void InitDatabase()
    {
        Debug.Log("Connect GameController of Puzzle02");
        GCP02 = GetComponent<GameControllerPuzzle02>();

        refreshDatabase();
    }
  
    public void refreshDatabase()
    {
        t1.values = new int[] { 15, 0, 0, 15 };
        t2.values = new int[] { -21, 24, -30, 51 };
        t3.values = new int[] { -30, -21, 51, 24 };
        t4.values = new int[] { -3, 4, -60, 12 };
        t5.values = new int[] { 3, 6, 23, 24 };
        t6.values = new int[] { 15, -18, 16, -19 };

        transforms = new transformMatrix[] { t1, t2, t3, t4, t5, t6 };
    }

    public int[] calculation(int[] vector, int[] matrix)
    {
        int[] newVectore = new int[] { 0, 0 };

        int[] originalVector = new int[] { 0, 0 };
        originalVector = vector;

        int[] transform = new int[] { 0, 0, 0, 0 };
        transform = matrix;

        //transform Matrix 
        // 0,1
        // 2,3
        transform[0] = transform[0] * originalVector[0];
        transform[2] = transform[2] * originalVector[0];
        transform[1] = transform[1] * originalVector[1];
        transform[3] = transform[3] * originalVector[1];

        newVectore[0] = transform[0] + transform[1];
        newVectore[1] = transform[2] + transform[3];

        refreshDatabase();

        return newVectore;
    }
}
