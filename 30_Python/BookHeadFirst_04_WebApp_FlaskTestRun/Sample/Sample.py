from flask import Flask
app = Flask(__name__)

@app.route('/')
def hello() -> str:
    return 'Hello world from Flask!'
@app.route('/bye')
def bye() -> str:
    return 'Goodbye world from Flask!'
app.run()

#then browse 127.0.0.1:5000 or 127.0.0.1:5000/bye