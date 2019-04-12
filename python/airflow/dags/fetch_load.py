"""
    I want fetch some dog pictures from Flickr, and store in a Sqlite db.

    Let's split the process into 2 tasks just for testing down/up stream tasks:

    1. fetch json data
    2. load to a sqlite db

    ===Begin> On Windows, tried and failed. ===

    Run airflow test in a temp dir:

    $env:AIRFLOW_HOME = 'D:\bruno\temp\airflow'
    cp -recursive dags\* $env:AIRFLOW_HOME
    airflow initdb
    airflow list_dags
    airflow list_tasks 
    airflow test discover fetch '2019-04-11'
    ===End> On Windows, tried and failed. ===

    ## the following test on Linux/Docker was successful
    == Docker image from puckel/docker-airflow:1.10.2 on Linux===
    1. git clone https://github.com/puckel/docker-airflow.git
    2. cd docker-airflow
    3. cp fetch_load.py to dags/
    4. docker-compose -f docker-compose-LocalExecutor.yml up -d
    5. use web UI to trigger "disocover" DAG to run
    == Docker image from puckel/docker-airflow:1.10.2 on Linux===

    In real World, logic should be moved to a library or a separate module to make development and testing simpler.
    
"""

import os
import datetime
import requests
import sqlite3
import logging
import json

from airflow import DAG
from airflow.models import Variable, TaskInstance, DagRun
# from airflow.utils.db import provide_session
from airflow.configuration import AIRFLOW_HOME
from airflow.operators.python_operator import PythonOperator

# using Docker image from puckel/docker-airflow:1.10.2, AIRFLOW_HOME
airflow_home = AIRFLOW_HOME
flickr_url = 'https://api.flickr.com/services/feeds/photos_public.gne?tags=dog&format=json&nojsoncallback=1'
db_file = os.path.join(airflow_home, 'discover_dag.db')
json_file = os.path.join(airflow_home, 'flickr.json')
data_keys = ['title', 'link', 'media', 'date_taken', 'description', 'published', 'author', 'author_id', 'tags']

default_args = {'start_date': datetime.datetime(2019, 4, 11),
                'retries': 2,
                'retry_delay': datetime.timedelta(minutes=2),
                'email': [],
                'email_on_failure': True}


dag = DAG('discover',
          default_args=default_args,
          schedule_interval='30 */12 * * *',
          catchup=False
          )

def fetch(url,data_file, **kwargs):
    logging.info(kwargs)
    res = requests.get(url).json()
    recs = res['items']
    with open(data_file, 'w', encoding="utf-8") as fs:
        json.dump(recs, fs, ensure_ascii=False)
    return recs[0]


def load_db(dbfile, data_file, **kwargs):
    ins = 'insert into photos values(?,?,?, ?,?,?, ?,?,?)'
    logging.info(ins)
    run_ddl = not os.path.exists(dbfile)
    cn = sqlite3.connect(dbfile)
    cur = cn.cursor()
    if run_ddl:
        ddl = 'create table photos(title text,link text,media text,\
            date_taken text,description text,published text,author text,author_id text,tags text)'
        cur.execute(ddl)
        logging.info(dbfile)
    with open(data_file, 'r', encoding='utf-8') as fs:
        recs = json.load(fs)
        for rec in recs:
            rec['media'] = rec['media']['m']
            cur.execute(ins, (rec[x] for x in data_keys))
        cn.commit()

    


with dag:
    task_fetch = PythonOperator(task_id='fetch_data',
                               python_callable=fetch,
                               op_args = [flickr_url, json_file],
                            #    op_kwargs = {},
                               provide_context=True)
    # 
    task_load = PythonOperator(task_id='load_data',
                               python_callable=load_db,
                               op_args = [db_file, json_file],
                               provide_context=True)
    # set up load as fetch's downstream task
    task_fetch >> task_load
