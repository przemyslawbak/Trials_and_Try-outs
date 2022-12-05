spam = 0
while spam < 5:
    print('Hello, world.')
    spam = spam + 1

name = ''
while name != 'your name':
    print('Please type your name.')
    name = input()

print('Thank you!')

while True:
    print('Please type your name.')
    name = input()
    if name == 'your name':
        break

print('Thank you!')