import pandas as pd

url = 'https://www.biznesradar.pl/gielda/indeks:WIG20,12,2'
tables = pd.read_html(url) # Returns list of all tables on page
wig20_table = tables[0]

print(wig20_table)
print(wig20_table.dtypes)

weight_dictionary = {'pko': 0,
          'ale': 0,
          'kgh': 0,
          'pkn': 0,
          'pzu': 0,
          'peo': 0,
          'dnp': 0,
          'lpp': 0,
          'cdr': 0,
          'pgn': 0,
          'spl': 0,
          'cps': 0,
          'pge': 0,
          'opl': 0,
          'lts': 0,
          'acp': 0,
          'ccc': 0,
          'tpe': 0,
          'jsw': 0,
          'mrc': 0,
          }

for key, value in weight_dictionary.items():
    dupa =  wig20_table[wig20_table['Profil'].str.contains(key.upper())]
    test = str(dupa['Udział'].values[0])[:-1]
    weight_dictionary[key] = float(test) / 100

for key, value in weight_dictionary.items():
    print(value)