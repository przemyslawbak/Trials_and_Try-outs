import pandas as pd
import warnings

warnings.filterwarnings("ignore")
pd.set_option('display.max_rows', 1000)
pd.set_option('display.max_columns', 10)
pd.set_option('display.width', 1000)

#slicing test data
def sliceData(mlDataFrame, SLICE_LENGTH, TICKS_BACKWARDS, PRED_QTY, mode):
    print('')
    print('Slicing data...')
    print('')
    df_length = mlDataFrame.shape[0] #i6467
    days_of_test = round(TICKS_BACKWARDS / 9) #10
    days_of_spread = round(days_of_test / PRED_QTY) #1
    spread_ticks = days_of_spread * 9 #spread between slices #9
    slicesQty = round((mlDataFrame.shape[0] - SLICE_LENGTH) / spread_ticks) #compute how many slice iterations for given 'mlDataFrame', is slicesQty == PRED_QTY?
    if mode == 'signals':
        slicesQty = PRED_QTY #10
    listDf = [] #init list

    for i in range(slicesQty):
        to_remove_first_rows = spread_ticks * (i) #9
        sliceFrom = df_length - (SLICE_LENGTH + to_remove_first_rows)
        sliceTo = df_length - to_remove_first_rows
        df_slice = mlDataFrame.iloc[sliceFrom:sliceTo] #removing first n rows and getting slice with 'head'
        print(df_slice)
        listDf.append(df_slice) #add slice to the list
        
    listDf = list(reversed(listDf))

    return listDf, days_of_test, days_of_spread, slicesQty

#Import df
filename = "GPW_DLY WIG20, 15.csv"
df = pd.read_csv(filename)

print(df.tail(1000))

SLICE_LENGTH = 5000 #or 5000
TICKS_BACKWARDS = 90 #or 8000
PRED_QTY = 10 #or 888
mode = 'signals'

slicedDfList, days_of_test, days_of_spread, slicesQty = sliceData(df, SLICE_LENGTH, TICKS_BACKWARDS, PRED_QTY, mode)


print('finito')