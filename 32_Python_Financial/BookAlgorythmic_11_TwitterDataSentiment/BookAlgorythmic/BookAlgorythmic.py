from pathlib import Path
import requests
from io import BytesIO
from zipfile import ZipFile

path = Path('sentiment140')
if not path.exists():
    path.mkdir()

URL = 'http://cs.stanford.edu/people/alecmgo/trainingandtestdata.zip'

response = requests.get(URL).content

with ZipFile(BytesIO(response)) as zip_file:
    for i, file in enumerate(zip_file.namelist()):
        if file.startswith('train'):
            local_file = path / 'train.csv'
        elif file.startswith('test'):
            local_file = path / 'test.csv'
        else:
            continue
        with local_file.open('wb') as output:
            for line in zip_file.open(file).readlines():
                output.write(line)