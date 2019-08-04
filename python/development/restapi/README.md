## Setting up a simple REST API server for testing

[flask restful](https://flask-restful.readthedocs.io/en/latest/quickstart.html)

### Set up a virtualenv using pipfile

Initialize project directory with pipenv install

```bash
pipenv install flask
pipenv install flask-restful
pipenv shell
python ./apy.py
```

### cURL tests
```bash
curl http://localhost:5000/todos
curl http://localhost:5000/todos/todo3
curl http://localhost:5000/todos/todo2 -X DELETE -v
curl http://localhost:5000/todos -d "task=something new" -X POST -v
curl http://localhost:5000/todos/todo3 -d "task=something different" -X PUT -v
```