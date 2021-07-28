﻿using System;
using System.Collections.Generic;
using System.IO;

using chapter03_logistic_regression.ML.Base;
using chapter03_logistic_regression.ML.Objects;

using Microsoft.ML;

namespace chapter03_logistic_regression.ML
{
    //You can think of NGrams as breaking a longer string into ranges of characters based on the
    //value of the NGram parameter.
    public class Trainer : BaseML
    {
        public void Train()
        {
            IEnumerable<FileInput> input = new FileInput[]
            {
                new FileInput { Label = false, Strings = @"!This program cannot be run in DOS mode.L$ SUVWH\$ UVWAVAWH\$ VWAVHWATAUAVAWHA_AA]A\_l$ VWAVHt$ WATAUAVAWH A_AA]A\_T$8H!\$8UVWATAUAVAWHF0D8#ukD8c`A_AA]A\_]WATAUAVAWHA_AA]A\_UATAUAVAWHA_AA]A\]h UAVAWHp WAVAWHWATAUAVAWHA_AA]A\_L$ UVWATAUAVAWHpA_AA]A\_]x UATAUAVAWHA_AA]A\]UVWATAUAVAWHA_AA]A\_]@SUVWAVAWH{8D8{@uOHUVWATAUAVAWHA_AA]A\_]UVWAVAWHx UAVAWHt$ UWAUAVAWHT$0H!\$0HH!~(H!~0H!~8H!~@3t$ UWATAVAWHUWAUAVAWHT$$D!t$ Hl$ VWAVHx UAVAWHx UAVAWHUVWATAUAVAWHA_AA]A\_]UATAUAVAWHA_AA]A\]@USVWATAUAVAWHA_AA]A\_[]UWATAVAWH@USVWATAVAWH0A_AA\_[]x ATAVAWHUVWAVAWHUVWAVAWHUVWAVAWHUVWAVAWHUVWAVAWHUVWAVAWHUVWAVAWHUVWAVAWHt$ UWATAVAWHD$@H9D$8u\$ UVWATAUAVAWHA_AA]A\_]x ATAVAWHWATAUAVAWHA_AA]A\_x UATAUAVAWHL$HtJ95zD9t$Ht	HA_AA]A\]l$HD9l$TuWATAUAVAWHA_AA]A\_UVWATAUAVAWHA_AA]A\_]WATAUAVAWH A_AA]A\_@USVWATAUAVAWHA_AA]A\_[]d$hD9d$xv?HUVWAVAWHUVWATAUAVAWH@A_AA]A\_]L$H9L$@vPHUWATAVAWHt$ WAVAWHx UATAUAVAWHA_AA]A\]|$ UATAUAVAWHA_AA]A\]VWATAVAWHUVWATAUAVAWH@A_AA]A\_]@SUVWAVHT$p+T$hLD$p+D$h9D$xu]Lt$ UWAVHUSVWATAVAWH`A_AA\_[] H3E H3EADVAPI32.dllCOMDLG32.dllPROPSYS.dllSHELL32.dllWINSPOOL.DRVurlmon.dllExceptionReturnHrFailFastRaiseFailFastExceptioninternal\sdk\inc\wil\opensource\wil\resource.hWilError_02RtlNtStatusToDosErrorNoTebRtlDllShutdownInProgressNtQueryWnfStateDataNtUpdateWnfStateDataRtlSubscribeWnfStateChangeNotificationRtlUnsubscribeWnfNotificationWaitForCompletioninternal\sdk\inc\wil/Staging.hWilStaging_02shell\osshell\accesory\notepad\notepad.cppshell\osshell\accesory\notepad\filesystemhelpers.hinternal\sdk\inc\wil\opensource/wil/win32_helpers.hinternal\sdk\inc\wil\opensource/wil/filesystem.hz?801i:It6NtQuerySystemInformationshell\osshell\accesory\common\edpapphelper\edpapphelper.cppEncodingSelectionSessionIdSequenceNumberEncodingPageSetupUpdatedSessionIdSequenceNumberHasHeaderOrFooterViewHelpSessionIdSequenceNumberTimeDateInvokedSessionIdSequenceNumberIsLogEntrySearchBingInvokedSessionIdSequenceNumberentrypointhasQueryTextFreshWindowSessionIdSequenceNumberIsAdminModeSessionIdSequenceNumberFileNewCountFileSaveCountFileSaveAsCountFilePrintCountEditUndoCountEditCutCountEditCopyCountEditPasteCountEditDeleteCountEditFindCountEditReplaceCountEditGotoCountFormatFontCountEdpFileOpenCountEdpFileSaveCountEdpPasteToNoContextCountEdpFileOpenAttemptFailCountFileSizeIsWordWrapStatusBarVisibilitySaveCompleteSessionIdSequenceNumberSaveCompleteSessionIdSequenceNumberContentTypeFileExtensionFileSizeIsNetworkPathEncodingSaveStartSessionIdSequenceNumberFileOpenCompleteSessionIdSequenceNumberContentTypeFileExtensionFileSizeIsNetworkPathEncodingFileOpenStartSessionIdSequenceNumberLaunchNotepadCompleteSessionIdSequenceNumberIsAdminModeLaunchNotepadStartSessionIdSequenceNumberMicrosoft.Notepadnotepad.pdb.text$di.text$mn.text$mn$00.text$yd.rdata$brc.rdata$T$brc.idata$5.CRT$XCA.CRT$XCAA.CRT$XCU.CRT$XCZ.CRT$XIA.CRT$XIAA.CRT$XIY.CRT$XIZ.CRT$XLA.CRT$XLZ.rdata$zETW0.rdata$zETW1.rdata$zETW2.rdata$zETW9.rdata$zzzdbg.tls$ZZZ.didat$2.didat$3.didat$4.didat$6.didat$7.idata$2.idata$3.idata$4.idata$6.data$brc.didat$5.rsrc$01.rsrc$02DuplicateEncryptionInfoFileDecryptFileWCommDlgExtendedErrorGetOpenFileNameWGetSaveFileNameWReplaceTextWFindTextWChooseFontWPageSetupDlgWGetFileTitleWPrintDlgExWPSGetPropertyDescriptionListFromStringPropVariantToStringVectorAllocSHCreateItemFromParsingNameShellExecuteWShellAboutWDragQueryFileWDragFinishSHAddToRecentDocsDragAcceptFilesOpenPrinterWGetPrinterDriverWClosePrinterFindMimeFromDataGetDeviceCapsCreateFontIndirectWDeleteObjectSelectObjectGetTextFaceWEnumFontsWTextOutWGetTextExtentPoint32WSetMapModeSetViewportExtExSetWindowExtExSetBkModeGetTextMetricsWAbortDocDeleteDCSetAbortProcStartDocWStartPageCreateDCWGDI32.dllGetClientRectMoveWindowSendMessageWSetThreadDpiAwarenessContextPostMessageWDialogBoxParamWGetFocusMessageBoxWCheckMenuItemGetSubMenuEnableMenuItemShowWindowReleaseDCSetCursorGetDpiForWindowSetActiveWindowLoadStringWDefWindowProcWIsIconicSetFocusPostQuitMessageDestroyWindowMessageBeepGetForegroundWindowGetDlgCtrlIDSetWindowPosRedrawWindowGetKeyboardLayoutCharNextWSetWinEventHookGetMessageWTranslateAcceleratorWIsDialogMessageWTranslateMessageDispatchMessageWUnhookWinEventSetWindowTextWOpenClipboardIsClipboardFormatAvailableCloseClipboardSetDlgItemTextWGetDlgItemTextWEndDialogSendDlgItemMessageWSetScrollPosInvalidateRectUpdateWindowGetWindowPlacementSetWindowPlacementCharUpperWGetSystemMenuLoadAcceleratorsWSetWindowLongWCreateWindowExWMonitorFromWindowRegisterWindowMessageWLoadCursorWRegisterClassExWGetWindowTextLengthWGetWindowLongWPeekMessageWGetWindowTextWEnableWindowCreateDialogParamWDrawTextExWUSER32.dll_vsnwprintfmemcpy_s_purecallmemmove_s__C_specific_handler_wcsicmpiswdigit_XcptFilter_amsg_exit__getmainargs__set_app_type_ismbblead__setusermatherr_initterm_commodemsvcrt.dll__dllonexit?terminate@@YAXXZGetModuleFileNameACreateSemaphoreExWHeapFreeSetLastErrorEnterCriticalSectionReleaseSemaphoreGetModuleHandleExWLeaveCriticalSectionInitializeCriticalSectionExWaitForThreadpoolTimerCallbacksWaitForSingleObjectGetCurrentThreadIdReleaseMutexFormatMessageWGetLastErrorReleaseSRWLockExclusiveOutputDebugStringWCloseThreadpoolTimerAcquireSRWLockExclusiveWaitForSingleObjectExOpenSemaphoreWCloseHandleSetThreadpoolTimerReleaseSRWLockSharedCreateThreadpoolTimerHeapAllocGetProcAddressCreateMutexExWAcquireSRWLockSharedDeleteCriticalSectionGetCurrentProcessIdGetProcessHeapGetModuleHandleWDebugBreakIsDebuggerPresentCoTaskMemFreeGlobalFreeGetLocaleInfoWCreateFileWReadFileCoTaskMemAllocCoCreateInstancePathIsFileSpecWSHStrDupWOpenProcessTokenGetCurrentProcessGetTokenInformationGetCommandLineWHeapSetInformationCoInitializeExFreeLibraryCoUninitializeFindFirstFileWFindCloseCompareStringOrdinalLocalAllocLocalFreeFoldStringWGetModuleFileNameWGetUserDefaultUILanguageGetLocalTimeGetDateFormatWGetTimeFormatWWideCharToMultiByteWriteFileGetFileAttributesWPathFileExistsWLocalLockLocalUnlockDeleteFileWSetEndOfFilePathFindExtensionWGetFileAttributesExWGetFileInformationByHandleCreateFileMappingWMapViewOfFileMultiByteToWideCharLocalReAllocUnmapViewOfFileGetFullPathNameWRegSetValueExWRegQueryValueExWRegCreateKeyWRegCloseKeyRegOpenKeyExWLocalSizeGetStartupInfoWGetDpiForMonitorlstrcmpiWFindNLSStringGlobalLockGlobalUnlockGlobalAllocCoCreateGuidEventSetInformationEventRegisterEventUnregisterEventWriteTransferIsTextUnicodeRtlCaptureContextRtlLookupFunctionEntryRtlVirtualUnwindUnhandledExceptionFilterSetUnhandledExceptionFilterTerminateProcessWakeAllConditionVariableSleepConditionVariableSRWQueryPerformanceCounterGetSystemTimeAsFileTimeGetTickCountGetProcessMitigationPolicyLoadLibraryExWPropVariantClearWindowsCreateStringCreateEventExWSetRestrictedErrorInfoCoWaitForMultipleHandlesCoCreateFreeThreadedMarshalerWindowsCreateStringReferenceRoGetActivationFactoryRoGetMatchingRestrictedErrorInfoSetEventWindowsDeleteStringRaiseExceptionWindowsGetStringRawBufferRoInitializeRoUninitializeapimswincorelibraryloaderl120.dllapimswincoresynchl110.dllapimswincoreheapl110.dllapimswincoreerrorhandlingl110.dllapimswincorethreadpooll120.dllapimswincoreprocessthreadsl110.dllapimswincorelocalizationl120.dllapimswincoredebugl110.dllapimswincorehandlel110.dllapimswincorecoml110.dllapimswincoreheapl210.dllapimswincorefilel110.dllapimswincoreshlwapilegacyl110.dllapimswincorelargeintegerl110.dllapimswinshcoreobsoletel110.dllapimswinsecuritybasel110.dllapimswincoreprocessenvironmentl110.dllapimswincorestringl110.dllapimswincorelocalizationobsoletel120.dllapimswincoresysinfol110.dllapimswincoredatetimel110.dllapimswinshcorepathl110.dllapimswincorememoryl110.dllapimswincoreregistryl110.dllapimswincoreregistryl210.dllapimswincoreheapobsoletel110.dllapimswinshcorescalingl111.dllapimswincorestringobsoletel110.dllapimswineventingproviderl110.dllapimswinbaseutill110.dllapimswincoresynchl120.dllapimswincorertlsupportl110.dllapimswincoreprofilel110.dllapimswincoreprocessthreadsl111.dllapimswincorewinrtstringl110.dllapimswincorewinrterrorl110.dllapimswincorewinrtl110.dllapimswincorewinrterrorl111.dllLoadImageWLoadIconWCreateStatusWindowWCOMCTL32.dll__CxxFrameHandler3ResolveDelayLoadedAPIDelayLoadFailureHookapimswincoredelayloadl111.dllapimswincoredelayloadl110.dll_callnewhew|>&=4_<?xml version" },
                new FileInput { Label = true, Strings = @"__gmon_startN_easy_cKcxa_amxBZNSt8ios_bEe4IeD1Evxxe6naDtqv_Z<4endlIcgLSaQ6appw3d_ResumeCXXABI_1.3%d by CWGttp://cwg. PROT_EXEC|PROT_WRITE failed.$Info: This file is packed with the UPX executable packer http://upx.sf.net $$Id: UPX 3.94 Copyright (C 19962017 the UPX Team. All Rights Reserved. $/proc/self/exeGCC: (Ubuntu 7.3.016u>crts;ff.c_tm_clones	tors_aux5ompled.7696_`$_finipp'ZStL19piec}k%4aticii0ii/GLOBAL2X_I_aFRAME_END_QFFSET_TABLE9DYNGIC1GmNUNHWHDREv@@pIBCXX_3\xX?oZt" },
                new FileInput { Label = true, Strings = @"!This program cannot be run in DOS mode._New_ptr7(_MaskQAlloc_maxtEqx?$xjinvalid argumC:\Program Files (x86\Micsoft Visur Studio\2019afRs0\VC\ToolsdS{m{GvK> Owne:by PE32+7$ENTRf fcUNl.>TAFafu(D:\jcgit\cwg\srcPE+\Debug'gerP Yj?ByteToWide&=(Unif}ndled{Fi8n6{{p5T.mina<#AorFe0eaZKXIdSys>msTNm@DU_Q@1@Ap?5_QAEAAV0vAH@Z~flk_`JolfpB.0J68(:6YQYu&pogkEe.poW~eR?d3M#6=%d6YE1pWDeTTVPW_O}]=rV}]mC]]HE}]]d\}<?xml version='1.0' encoding='UTF8' standalone='yes'?><assembly xmlns='urn:schemasmicrosoftcom:asm.v1' manifestVersion='1.0'>  <trustInfo xmlns=urn:schemasmicrosoftcom:asm.v3>    <security>      <requestedPrivileges>        <requestedExecutionLevel level='asInvoker' uiAccess='false' />      </requestedPrivileges>    </security>  </trustInfo></assembly>KERNEL32.DLLMSVCP140D.dllucrtbased.dllVCRUNTIME140D.dllExitProcessGetProcAddressLoadLibraryAVirtualProtect??1_Lockit@std@@QAE@XZ" },
            };

            IDataView trainingDataView = MlContext.Data.LoadFromEnumerable(input);

            DataOperationsCatalog.TrainTestData dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ColumnConcatenatingTransformer> dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(FileInput.Label))
                .Append(MlContext.Transforms.Text.FeaturizeText("NGrams", nameof(FileInput.Strings)))
                .Append(MlContext.Transforms.Concatenate("Features", "NGrams"));

            Microsoft.ML.Trainers.SdcaLogisticRegressionBinaryTrainer trainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");

            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.LinearBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);
        }
    }
}