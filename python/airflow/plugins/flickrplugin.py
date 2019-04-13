"""
    airflow plugin test

    ```
    from airflow.operators import FlickrOperator

    flickr_task = FlickrOperator(
        task_id='get_photo_task',
        flickr_url='https://api.flickr.com/services/feeds/photos_public.gne?tags=dog&format=json&nojsoncallback=1',
        sqlite_db_file="get_photo_task.db",
        dag=dag)
    ```
"""

import os
import datetime
import requests
import sqlite3
import logging
import json


from airflow.plugins_manager import AirflowPlugin
from airflow.models import BaseOperator
from airflow.utils import apply_defaults
from airflow.configuration import AIRFLOW_HOME

data_keys = ['title', 'link', 'media', 'date_taken', 'description', 'published', 'author', 'author_id', 'tags']

class FlickrOperator(BaseOperator):
    """
    A test operator to do an http request and save the results in a Sqlite db.
    :param flickr_url: url
    :type flickr_url: string
    :param sqlite_db_file: sqlite db file name
    :type sqlite_db_file: string
    """

    template_fields = ('flickr_url', 'sqlite_db_file')
    template_ext = []
    ui_color = '#ffffff'  # ZipOperator's Main Color: white  # todo: find better color

    @apply_defaults
    def __init__(
            self,
            flickr_url, 
            sqlite_db_file,
            *args, **kwargs):
        self.flickr_url = flickr_url
        self.sqlite_db_file = os.path.join(AIRFLOW_HOME, sqlite_db_file)
        super().__init__(*args, **kwargs)

    def execute(self, context):
        """ simple http get without error checking """
        res = requests.get(self.flickr_url).json()
        recs = res['items']
        ins = 'insert into photos values(?,?,?, ?,?,?, ?,?,?)'
        logging.info(ins)
        dbfile = self.sqlite_db_file
        run_ddl = not os.path.exists(dbfile)
        cn = sqlite3.connect(dbfile)
        cur = cn.cursor()
        total = 0
        if run_ddl:
            ddl = 'create table photos(title text,link text,media text,\
                date_taken text,description text,published text,author text,author_id text,tags text)'
            cur.execute(ddl)
            logging.info(dbfile)
        for rec in recs:
            rec['media'] = rec['media']['m']
            cur.execute(ins, [rec[x] for x in data_keys])
            total += 1
        cn.commit()
        return 'Total {} records appended to photos table'.format(total)


# Defining the plugin class
class FlickrPlugin(AirflowPlugin):
    name = "flickr_plugin"
    operators = [FlickrOperator]
    flask_blueprints = []
    hooks = []
    executors = []
    admin_views = []
    menu_links = []