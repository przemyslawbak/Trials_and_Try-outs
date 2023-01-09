import models

iters = [1, 2, 3]
some_object = models.SomeModel()

for i in iters:
    prop_name = 'attr_' + str(i)
    setattr(some_object, prop_name, str(i))
    
print(some_object.attr_1)
print(some_object.attr_2)
print(some_object.attr_3)