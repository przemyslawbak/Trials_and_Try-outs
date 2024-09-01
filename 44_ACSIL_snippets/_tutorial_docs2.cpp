#include "sierrachart.h"
SCDLLName("Custom Study DLL") 

//NOT WPRKING

//source: https://www.sierrachart.com/index.php?page=doc/AdvancedCustomStudyInterfaceAndLanguage.php

SCSFExport scsf_IsUserAllowedForSCDLLNameExample(SCStudyInterfaceRef sc)
{

  if (sc.SetDefaults)
  {
    // Set the configuration and defaults

    sc.GraphName = "IsUserAllowedForSCDLLName";
    sc.StudyDescription = "This function is an example of using the sc.IsUserAllowedForSCDLLName variable to protect a study.";
    sc.AutoLoop = 1;


    return;
  }


  // Do data processing

  if(sc.IsUserAllowedForSCDLLName == false)
  {

    if(sc.Index == 0)
    {
      sc.AddMessageToLog("You are not allowed to use this study",1);
    }

    return;
  }
} 