from flask import Flask
app = Flask(__name__)
@app.route('/')
def hello() -> str:
    return 'Hello world from Flask!'
app.run()

#then browse 127.0.0.1:5000