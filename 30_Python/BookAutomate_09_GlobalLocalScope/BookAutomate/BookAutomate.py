eggs = 'global eggs'

def spam():
    eggs = 'spam local'
    print(eggs) # prints 'spam local'
def bacon():
    eggs = 'bacon local'
    print(eggs) # prints 'bacon local'

spam()
bacon()
print(eggs) # prints 'global'

if True:
    testLocal = 'test global if'

print(testLocal) # prints 'test global if'