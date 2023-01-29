import unittest
import inspect

#Multiple Test Classes Within the Same Test File/Module
class TestClass03(unittest.TestCase):
    def test_case03(self):
        print("\nClassname : " + self.__class__.__name__)
        print("Running Test Method : " + inspect.stack()[0][3])
        
class TestClass02(unittest.TestCase):
    def test_case02(self):
        print("\nClassname : " + self.__class__.__name__)
        print("Running Test Method : " + inspect.stack()[0][3])

if __name__ == '__main__':
    unittest.main()