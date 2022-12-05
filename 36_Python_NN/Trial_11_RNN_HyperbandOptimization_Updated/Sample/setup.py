from distutils.core import setup
import py2exe

#Python is usually not compiled but executed using the python interpreter. https://stackoverflow.com/a/47405841
setup(console=['Sample.py'])