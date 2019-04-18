
"""
    dbapi 2.0 compliant drivers for Python

"""


def connect(driver_mod, conn_str):
    """

    :param driver_mod: str, examples: sqlite3, psycopg2, cx_Oracle, ...
    :param conn_str: str, driver-specific, example for Oracle: scott/tiger@mytnsname
    :return: dbapi 2.0 connection
    """
    cn = None
    try:
        driver = __import__(driver_mod)
        cn = driver.connect(conn_str)
    except ImportError as e:
        print(e)
        raise e
    return cn

