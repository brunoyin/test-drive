"""
    Copy all records between 2 identical tables using sqlalchemy:
    
    1. generate select query
    2. generate insert query
    3. use batches to reduce resource usage on database servers
"""
import traceback

import sqlalchemy
from sqlalchemy import create_engine, MetaData, Table
from sqlalchemy.orm import sessionmaker
# from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.sql import select
from sqlalchemy.schema import CreateTable

ARRAY_SIZE = 1000

def make_session(connection_string, engine_options={}):
    # options = engine_options or {}
    options = {}
    # todo: what other sqlalchemy supported db not use those properties?
    if not connection_string.startswith('sqlite'):
        options.update(engine_options, convert_unicode=True, coerce_to_decimal=True)

    engine = create_engine(connection_string, **options)
    session = sessionmaker(bind=engine)
    return session(), engine

def copy_table(from_db, from_table, to_db, to_table, 
    from_owner=None, to_owner=None, engine_options={}):
    """
        :param from_db, string, source sqlalchemy db connection url
        :param from_table, string, source table name
        :param to_db, string, destination sqlalchemy db connection url
        :param to_table, string, destination table name
        :param from_owner, optional, string, schema name of source table
        :param to_owner, optional, string, schema name of destination table
    """
    options = {'arraysize': ARRAY_SIZE}
    engine_options.update(options)
    source, sengine = make_session(from_db, engine_options)
    dest, dengine = make_session(to_db, engine_options)
    smeta = MetaData()
    dmeta = MetaData()

    total = 0

    try:
        source_tbl = Table(from_table, smeta, autoload=True, schema=from_owner,autoload_with=sengine)
        dest_tbl = Table(to_table, dmeta, autoload=True, schema=to_owner,autoload_with=dengine)
        # 
        sel_query = select([source_tbl])
        insert_sql = dest_tbl.insert()
        # 
        results = source.execute(sel_query)
        while True:
            recs = results.fetchmany(ARRAY_SIZE)
            _total = len(recs)
            total += _total
            if _total > 0:
                dest.execute(insert_sql, recs)
                dest.commit()
            if _total < ARRAY_SIZE:
                break
        # done
        print(f'{total} records copied from {from_table} to {to_table}')
    except sqlalchemy.exc.NoSuchTableError as e1:
        # traceback.print_exc()
        print(e1)
    except TypeError as e2:
        # traceback.print_exc()
        print(e2)
    except Exception as e3:
        traceback.print_exc()
        print(e3)
    return total
    
