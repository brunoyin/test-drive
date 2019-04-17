
import os 
import sqlite3

import pytest

# 
import sacopy

_test_root = os.path.dirname(os.path.abspath(__file__))

table_name = 'test_table'
table_ddl = f'create table {table_name}(col1 text, col2 text)'

def create_db(dbfile, populate_test_data=False):
    if os.path.exists(dbfile):
        os.remove(dbfile)
    cn = sqlite3.connect(dbfile)
    cur = cn.cursor()
    cur.execute(table_ddl)
    if populate_test_data:
        ins_query = f'insert into {table_name} values(?, ?)'
        for x in range(100):
            cur.execute(ins_query, ['col1: {:03d}'.format(x), 'col2: {:03d}'.format(100-x) ])
    cn.commit()
    cn.close()
    return f"sqlite:///{dbfile}"

@pytest.fixture
def source_db():
    dbfile = os.path.join(_test_root, 'test_source.db')
    return create_db(dbfile, populate_test_data=True)

@pytest.fixture
def dest_db():
    dbfile = os.path.join(_test_root, 'test_dest.db')
    return create_db(dbfile, populate_test_data=False)

def test_copy(source_db, dest_db):
    ret = sacopy.copy_table(from_db=source_db, from_table=table_name,
        to_db=dest_db, to_table=table_name)
    assert ret == 100
