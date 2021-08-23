from flask import Flask, render_template
app = Flask(__name__)
@app.route('/')
def entry_page() -> 'html':
    return render_template('entry.html', the_title='Welcome to search4letters on the web!')      
@app.route('/search4', methods=['POST'])
def do_search() -> str:
    return str(search4letters('life, the universe, and everything', 'eiru,!'))
app.run(debug = True)

#!!!!!!!!not completed