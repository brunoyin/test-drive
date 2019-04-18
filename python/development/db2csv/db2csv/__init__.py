__version__ = '0.1.0'


import csv
import click

from .dbconnect import connect


class Db2csvException(Exception):
    pass


def export(cn, query_text, out_csvfile, parameters=[],  need_header=True, batch_size=1000):
    success = False
    total = 0

    try:
        cur = cn.cursor()
        if len(parameters) > 0:
            cur.execute(query_text, parameters)
        else:
            cur.execute(query_text)
        with open(out_csvfile, 'w', newline='') as fs:
            w = csv.writer(fs)
            if need_header:
                w.writerow([x[0] for x in cur.description])
            while True:
                recs = cur.fetchmany(batch_size)
                _total = len(recs)
                total += _total
                if _total > 0:
                    w.writerows(recs)
                if _total < batch_size:
                    break
        cur.close()
        success = True
    except Exception as e:
        print(e)
        raise e

    return success, total


@click.command()
@click.option('-c', '--conn-str', help='driver specific connection string')
@click.option('-d', '--driver', help='db driver module. Examples: cx_Oracle, sqlite3, psycopg2')
@click.option('-qt', '--query-text', default=None, help='select query that may include parameters')
@click.option('-qf', '--query-file', default=None,help='filename with select query that may include parameters')
@click.option('-o', '--out-file', help='csv filename to be written to')
@click.option('-p', '--parameters', multiple=True,
              help='multiple parameters. Must be in the same order as they appear in query mark(?)')
def main(conn_str, driver, query_text, query_file, out_file, parameters):
    cn = None
    success, total = False, 0
    try:
        cn = connect(driver, conn_str)
        if query_text is None:
            with open(query_file, 'r') as fs:
                query_text = fs.read()
        success, total = export(cn, query_text, out_file, parameters=parameters)
    except Exception as e:
        print(e)
    finally:
        if cn is not None:
            try:
                cn.close()
            except: # progma: no cover
                pass
    if success:
        print(f"{total} records exported to {out_file}")


if __name__ == '__main__':
    main() # progma: no cover

