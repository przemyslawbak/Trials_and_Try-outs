import os

def checkIfFileExists(filePath):
    return os.path.isfile(filePath)

def saveNewLine(text, path, mode):
    hs = open(path, mode)
    hs.write(text + "\n")
    hs.close()

def deleteFile(file_path):
    try:
        os.remove(file_path)
    except OSError:
        pass

prediction_counter_path = 'ml_tests/prediction_counter.csv'
text = 'some_text'
rng = 10

for i in range(rng):
    read_text = 'some_other_text'
    read_no = 0

    if checkIfFileExists(prediction_counter_path):
        prediction_counter_line = ''
        infile = open(prediction_counter_path, 'r')
        prediction_counter_line = infile.readline()

    deleteFile(prediction_counter_path)
    predictions_line = str(i) + '|' + text + str(i)
    saveNewLine(predictions_line, prediction_counter_path, "a")
