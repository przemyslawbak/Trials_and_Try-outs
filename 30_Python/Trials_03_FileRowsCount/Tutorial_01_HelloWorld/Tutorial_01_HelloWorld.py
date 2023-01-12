import os

def checkIfFileExists(file_name):
    return os.path.isfile(file_name)

def checkIfShouldProceedWithTests(testResultPath, slicesQty):
    some_file = open(testResultPath)
    num_lines = sum(1 for line in some_file)
    some_file.close()

    #2 rows for header and info
    if slicesQty > (num_lines - 2):
        return True

    return False

def checkCompletedPredictionsNo():
    #todo:
    return 0


slicesQty = 848
testResultPath = 'ml_tests/training-1h-Core-hourlyTrueTrueTrue0.2195003210.215lstmrelulinear5000-col-C-H-M-S-P-I-U-W-P-C-M-S-P-C-M-S-P-C-M-S-P-C-training-50008000888.txt'

should_proceed = checkIfShouldProceedWithTests(testResultPath, slicesQty)

if should_proceed or not checkIfFileExists(testResultPath):
    predictions_completed = checkCompletedPredictionsNo()