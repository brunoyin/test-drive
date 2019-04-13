## Testing Apache Airflow

This is a nice schdeduling and automation tool. It makes use of some well known Python libraries and frameworks like Flask, Celery, Dask.

## [DAG](https://en.wikipedia.org/wiki/Directed_acyclic_graph): or job definition in Python

[fetch_load.py](./dags/fetch_load.py): work is getting done by an Operator. In this test:

- PythonOperator is used to execute a function
- For testing purpose, I created 2 functions to create 2 tasks with dependancy

## An Airflow plugin: [documentation](https://airflow.apache.org/plugins.html)
[flickrplugin.py](./plugins/flickrplugin.py)

- The main purpose is code organization and re-use
- In this test, I moved the Python code into the flickrplugin.py, a new DAG file [flickr_dag.py](./plugins/flickrplugin.py) is much more readable
- airflow automatically discovers the code in plugins directory, put all operators under "airflow.operators". For example, to import "FlickrOperator" defined in this test plugin, you need to do like this:
    ```python
    from airflow.operators import FlickrOperator
    ```

## Run a test in Docker

If you have VS C++ installed, you can get Airflow installed, but it does not work under Windows. It should work in a Linux subsystem on Windows.

Installation and running on Linux is simple and straightforward.

Running in Docker is even easier using the image from https://github.com/puckel/docker-airflow:

- git clone https://github.com/puckel/docker-airflow.git
- copy dags/fetch_load.py to the dags directory 
- copy plugins/flickr_dag.py to dags directory 
- copy plugins/flickrplugin.py to plugins directory 
- docker-compose -f docker-compose-LocalExecutor.yml up -d
- use a browser to open linux-host-ip:8080
- locate "discover" dag, toggle "On", the trigger it to run

[docker-compose-LocalExecutor.yml](./docker-compose-LocalExecutor.yml): 
```yml
version: '2.1'
services:
    postgres:
        image: postgres:9.6
        environment:
            - POSTGRES_USER=airflow
            - POSTGRES_PASSWORD=airflow
            - POSTGRES_DB=airflow

    webserver:
        image: puckel/docker-airflow:1.10.2
        restart: always
        depends_on:
            - postgres
        environment:
            - LOAD_EX=n
            - EXECUTOR=Local
        volumes:
            - ./dags:/usr/local/airflow/dags
            # Uncomment to include custom plugins
            - ./plugins:/usr/local/airflow/plugins
        ports:
            - "8080:8080"
        command: webserver
        healthcheck:
            test: ["CMD-SHELL", "[ -f /usr/local/airflow/airflow-webserver.pid ]"]
            interval: 30s
            timeout: 30s
            retries: 3
```
