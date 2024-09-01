#include "sierrachart.h"
SCDLLName("Custom Study DLL") 

//source: https://www.sierrachart.com/index.php?page=doc/AdvancedCustomStudyInterfaceAndLanguage.php

SCSFExport scsf_UniqueFunctionName(SCStudyInterfaceRef sc)
{
    if (sc.SetDefaults)
    {
        // Set the defaults
        sc.GraphName = "My New Study Function";

        sc.Subgraph[0].Name = "Subgraph name";
        sc.Subgraph[0].DrawStyle = DRAWSTYLE_LINE;

        sc.AutoLoop = 1;

        // Enter any additional configuration code here
        return;
    }

    // Perform your data processing here.

    // Multiply the Last price at the current bar being processed, by 10.
    sc.Subgraph[0][sc.Index] = sc.Close[sc.Index] * 10;

    return;
}