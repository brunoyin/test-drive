
import datetime
from airflow import DAG
from airflow.models import Variable, TaskInstance, DagRun
from airflow.operators.dummy_operator import DummyOperator


from airflow.operators import FlickrOperator

default_args = {'start_date': datetime.datetime(2019, 4, 11),
                'retries': 2,
                'retry_delay': datetime.timedelta(minutes=2),
                'email': [],
                'email_on_failure': True}


dag = DAG('flickr',
          default_args=default_args,
          schedule_interval='30 */12 * * *',
          catchup=False
          )

flickr_task = FlickrOperator(
    task_id='get_photo_task',
    flickr_url='https://api.flickr.com/services/feeds/photos_public.gne?tags=dog&format=json&nojsoncallback=1',
    sqlite_db_file="get_photo_task.db",
    dag=dag)

task1 = DummyOperator(
            task_id='start',
            dag=dag
        )

task1 >> flickr_task