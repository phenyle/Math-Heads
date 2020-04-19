using UnityEngine;

public struct transformMatrix
{
    public int[] values;
}

public class DatabasePuzzle02 : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;
    
    public transformMatrix[] transforms0;
    public transformMatrix[] transforms1;
    public transformMatrix[] transforms2;
    public transformMatrix[] transforms3;
    public transformMatrix[] transforms4;
    public transformMatrix[] transforms5;

    transformMatrix t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24;

    public int[,] tragetMatracies = new int[,] { {15, 15}, {-21, 24}, {-30 ,51}, {-7, 72}, {9, 47}, {33, 35} }; 

    public void InitDatabase()
    {
        Debug.Log("Connect GameController of Puzzle02");
        GCP02 = GetComponent<GameControllerPuzzle02>();

        refreshDatabase();
    }
  
    public void refreshDatabase()
    {
        t1.values = new int[]  { 15, 0, 0, 15 };
        t2.values = new int[]  { -15, 0, 0, -15 };
        t3.values = new int[]  { 0, 0, 15, 15 };
        t4.values = new int[]  { 15, 15, 15, 15 };
        //question 2
        t5.values = new int[]  { 0, 0, 0, 0 };
        t6.values = new int[]  { -21, 0, 0, 24 };
        t7.values = new int[] { 0, 0, 0, 0 };
        t8.values = new int[] { 0, 0, 0, 0 };
        //question 3
        t9.values = new int[] { -30, 0, 0, 51 };
        t10.values = new int[] { 0, 0, 0, 0 };
        t11.values = new int[] { 0, 0, 0, 0 };
        t12.values = new int[] { 0, 0, 0, 0 };
        //question 4
        t13.values = new int[] { -7, 0, 0, 72 };
        t14.values = new int[] { 0, 0, 0, 0 };
        t15.values = new int[] { 0, 0, 0, 0 };
        t16.values = new int[] { 0, 0, 0, 0 };
        //question 5
        t17.values = new int[] { 9, 0, 0, 47 };
        t18.values = new int[] { 0, 0, 0, 0 };
        t19.values = new int[] { 0, 0, 0, 0 };
        t20.values = new int[] { 0, 0, 0, 0 };
        //question 6
        t21.values = new int[] { 33, 0, 0, 35 };
        t22.values = new int[] { 0, 0, 0, 0 };
        t23.values = new int[] { 0, 0, 0, 0 };
        t24.values = new int[] { 0, 0, 0, 0 };

        transforms0 = new transformMatrix[] { t1, t2, t3, t4 };
        transforms1 = new transformMatrix[] { t5, t6, t7, t8 };
        transforms2 = new transformMatrix[] { t9, t10, t11, t12 };
        transforms3 = new transformMatrix[] { t13, t14, t15, t16 };
        transforms4 = new transformMatrix[] { t17, t18, t19, t20 };
        transforms5 = new transformMatrix[] { t21, t22, t23, t24 };
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
