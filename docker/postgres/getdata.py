import numpy as np
import pandas as pd
import csv

selected_cols = ['UNITID','INSTNM','CITY', 'STABBR','ZIP','REGION', 'LATITUDE', 'LONGITUDE',
    'ADM_RATE', 'SAT_AVG', 'ACTCMMID', 'MD_EARN_WNE_P10', 'COSTT4_A','UGDS']
float_cols = ['LATITUDE', 'LONGITUDE','ADM_RATE', 'SAT_AVG', 'ACTCMMID', 'MD_EARN_WNE_P10', 'COSTT4_A','UGDS']

def export2csv(src_csv, out_csv):
    df = pd.read_csv(src_csv, low_memory=False)
    df2 = df[df.PREDDEG.isin(['2', '3'])]
    df2[float_cols] = df2[float_cols].apply(pd.to_numeric, errors='coerce')
    df2.to_csv(out_csv, columns=selected_cols, index=False)

if __name__ == '__main__':
    import sys
    export2csv(sys.argv[1], sys.argv[2])
