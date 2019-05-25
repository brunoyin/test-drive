"""
    interview online assessment

    question 2: find the fastest delivering route

    given a map of 2d array,  example
    [
     1  0  0
     1  0  0
     1  9  0
    ]

    start from top left, delivery is marked as 9, no access 0, has access 1

    find the fastest path

    Test01:

     a = [
        [1, 0, 1, 1, 1],
        [1, 1, 1, 0, 1],
        [0, 1, 9, 0, 0],
        [1, 1, 1, 0, 0]
    ]


    [[(0, 0), (1, 0), (1, 1), (1, 2), (2, 2)],
     [(0, 0), (1, 0), (1, 1), (2, 1), (2, 2)],
     [(0, 0), (1, 0), (1, 1), (2, 1), (3, 1), (3, 2), (2, 2)]]

    ========
    [(0, 0), (1, 0), (1, 1), (1, 2), (2, 2)]
    4 = shortest path

"""

import copy
import pprint

total_paths = []

def make_copy(rec, pos):
    rec1 = copy.copy(rec)
    rec1.append(pos)
    return rec1

def traverse_node(numOfRows, numOfColumns, area, pos, rec=[]):
    x,y = pos
    if area[x][y] == 0:
        return
    # avoid visited
    if pos in rec:
        return
    if area[x][y] == 9:
        total_paths.append(make_copy(rec, pos))
        return
    # down
    if y + 1 < numOfColumns:
        traverse_node(numOfRows, numOfColumns, area, (x, y+1), rec=make_copy(rec, pos))
    if y - 1 >= 0:
        traverse_node(numOfRows, numOfColumns, area, (x, y-1), rec=make_copy(rec, pos))
    if x + 1 < numOfRows:
        traverse_node(numOfRows, numOfColumns, area, (x+1, y), rec=make_copy(rec, pos))
    if x - 1 >= 0:
        traverse_node(numOfRows, numOfColumns, area, (x-1, y), rec=make_copy(rec, pos))



def find_path(numOfRows, numOfColumns, area):
    traverse_node(numOfRows, numOfColumns, area, (0,0))
    pprint.pprint(total_paths)
    min_path, min_val = None, None
    for x in total_paths:
        v = len(x)
        # pprint.pprint(x)
        # print('length = {}\n'.format(v))
        if min_val is None:
            min_val = v
            min_path = x
        elif v < min_val:
            min_val = v
            min_path = x
    print('\n========')
    pprint.pprint(min_path)
    print('{} = shortest path'.format(min_val-1))
    return v - 1


if __name__ == '__main__':
    a = [
        [1, 0, 1, 1, 1],
        [1, 1, 1, 0, 1],
        [0, 1, 9, 0, 0],
        [1, 1, 1, 0, 0]
    ]
    rows, cols = 4, 5
    find_path(rows, cols, a)

