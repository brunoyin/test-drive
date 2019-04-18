
import os
import pytest


from db2csv import __version__, export, dbconnect

_test_root = os.path.dirname(os.path.abspath(__file__))


@pytest.fixture()
def conn():
    cn = dbconnect.connect('sqlite3', ':memory:')
    ddl = 'create table test_table(col1 text, col2 integer)'
    cur = cn.cursor()
    cur.execute(ddl)
    for x in range(100):
        cur.execute('insert into test_table values(?, ?)', ['col1-{}'.format(x), x])
    cn.commit()
    return cn


def test_export(conn):
    q = 'select * from test_table where col2 > ?'
    csvfile = os.path.join(_test_root, 'test_table.csv')
    success,total = export(conn, q, csvfile, parameters=[10])
    assert success
    assert total > 80

def test_version():
    assert __version__ == '0.1.0'



