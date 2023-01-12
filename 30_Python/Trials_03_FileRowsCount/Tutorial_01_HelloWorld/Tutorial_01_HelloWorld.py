import os

def checkIfFileExists(file_name):
    return os.path.isfile(file_name)

def checkIfShouldProceedWithTests(testResultPath, slicesQty):

    if checkIfFileExists(testResultPath):
        num_lines = checkCompletedPredictionsNo(testResultPath)

        if slicesQty > num_lines:

            return True

    return False

def checkCompletedPredictionsNo(testResultPath):
    if checkIfFileExists(testResultPath):
        some_file = open(testResultPath)
        num_lines = sum(1 for line in some_file)
        some_file.close()
        
        #2 rows for header and info
        return num_lines - 2

    return 0


slicesQty = 848 #245 in test file - 2 rows for headers and info
testResultPath = 'ml_tests/training-1h-Core-hourlyTrueTrueTrue0.2195003210.215lstmrelulinear5000-col-C-H-M-S-P-I-U-W-P-C-M-S-P-C-M-S-P-C-M-S-P-C-training-50008000888.txt'

should_proceed = checkIfShouldProceedWithTests(testResultPath, slicesQty)
file_exists = checkIfFileExists(testResultPath)

if should_proceed or not file_exists:
    predictions_completed = checkCompletedPredictionsNo(testResultPath)
    print('finito')