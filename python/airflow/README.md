## Testing Apache Airflow

This is a nice schdeduling and automation tool. It makes use of some well known Python libraries and frameworks like Flask, Celery, Dask.

## Run a test in Docker

If you have VS C++ installed, you can get Airflow installed, but it does not work under Windows. It should work in a Linux subsystem on Windows.

Installation and running on Linux is simple and straightforward.

Running in Docker is even easier using the image from https://github.com/puckel/docker-airflow:

- git clone https://github.com/puckel/docker-airflow.git
- copy the test DAG file into the dags directory 
- docker-compose -f docker-compose-LocalExecutor.yml up -d
- use a browser to open linux-host-ip:8080
- locate "discover" dag, toggle "On", the trigger it to run


