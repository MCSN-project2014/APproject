#!/bin/python
from flask import Flask
from flask import render_template
from flask import request
import subprocess
import platform

app = Flask(__name__)

@app.route('/', methods=['POST', 'GET'])
def main():
    if request.method == 'POST':
        dataRow = request.data

        if dataRow == '' and request.form.get('json') != None:
            dataRow = request.form['json']
        elif dataRow == '':
            return 'data error'

        data = dataRow.split('&')

        if platform.system() == 'Linux':
            comand = ['mono', 'bin/funwaps.exe', data[0], data[1]]
        else:
            comand = ['bin\\funwaps.exe', data[0], data[1]]

        p = subprocess.Popen(comand,stdout=subprocess.PIPE,stderr=subprocess.PIPE)
        out, err = p.communicate()
        if err == "":
            print('result: '+out)
            return out
        else:
            print('error: '+err)
            return "server internal error"

    # the code below is executed if the request method
    # was GET or the credentials were invalid
    return render_template('index.html')

if __name__ == "__main__":
    app.run(debug=True)
