#include "sierrachart.h"

//https://www.sierrachart.com/SupportBoard.php?ThreadID=33630

SCDLLName("Test to file") 

SCSFExport scsf_WriteBarDataToFile(SCStudyInterfaceRef sc)
{
	SCInputRef Input_Separator = sc.Input[0];
	SCInputRef Input_UseUTCTime = sc.Input[1];

	if (sc.SetDefaults)
	{
		sc.GraphName = "Write Bar Data To File";
		sc.StudyDescription = "Write Bar Data To File";

		sc.GraphRegion = 0;

		Input_Separator.Name = "Separator";
		Input_Separator.SetCustomInputStrings("Comma;Tab");
		Input_Separator.SetCustomInputIndex(0);

		Input_UseUTCTime.Name = "Use UTC Time";
		Input_UseUTCTime.SetYesNo(0);

		sc.TextInputName = "File Path";

		sc.AutoLoop = 0;//manual looping for efficiency
		return;
	}

	if (sc.LastCallToFunction)
		return;

	SCString OutputPathAndFileName;
	if (!sc.TextInput.IsEmpty())
	{
		OutputPathAndFileName = sc.TextInput;
	}
	else
	{
		OutputPathAndFileName = sc.DataFilesFolder();
		OutputPathAndFileName += "\\";
		OutputPathAndFileName += sc.Symbol.GetChars();
		OutputPathAndFileName += "-BarData.txt";
	}


	n_ACSIL::s_WriteBarAndStudyDataToFile WriteBarAndStudyDataToFileParams;
	WriteBarAndStudyDataToFileParams.StartingIndex = sc.UpdateStartIndex;
	WriteBarAndStudyDataToFileParams.OutputPathAndFileName = OutputPathAndFileName;
	WriteBarAndStudyDataToFileParams.IncludeHiddenStudies = 0;
	WriteBarAndStudyDataToFileParams.IncludeHiddenSubgraphs = 0;
	WriteBarAndStudyDataToFileParams.AppendOnlyToFile = 0;
	WriteBarAndStudyDataToFileParams.IncludeLastBar = 0;
	WriteBarAndStudyDataToFileParams.UseUTCTime = Input_UseUTCTime.GetYesNo();
	WriteBarAndStudyDataToFileParams.WriteStudyData = 0;
	WriteBarAndStudyDataToFileParams.UseTabDelimiter = Input_Separator.GetInt() == 1;
	sc.WriteBarAndStudyDataToFileEx(WriteBarAndStudyDataToFileParams);
}