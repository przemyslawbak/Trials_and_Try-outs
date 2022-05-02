using System;

class Solution
{
    static void Main(String[] args)
    {
        string path = "DDUUUUDD";

        char[] arr = path.ToCharArray();

        int elevation = 0;
        int valleys = 0;

        foreach (char step in arr)
        {
            int currentStep = step.Equals('U') ? 1 : -1;

            if (elevation < 0 && (elevation + currentStep) == 0)
            {
                valleys++;
            }

            elevation += currentStep;
        }

        //return valleys
    }
}
