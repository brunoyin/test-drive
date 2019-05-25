"""
    job interview online assessment:

        parts assembly and required time

        problem:

        1. parts sizes are in an int array like [10,6,4, 20]
        2. assemble 2 parts at a time, a combined one will have the size = part1 size + part2 size
        3. time required = part1 size + part2 size
        4. return the minimum time required

"""

def get_requiredtime(parts):
    total_time = 0
    remaining_parts = parts
    while len(remaining_parts) > 1:
        x1 = remaining_parts[0]
        x2 = remaining_parts[1]
        new_parts = x1 + x2
        total_time += x1 + x2
        if len(remaining_parts) == 2:
            # you just finished
            break
        remaining_parts = [new_parts] + remaining_parts[2:]
    return total_time

if __name__ == '__main__':
    # test case 1
    test1 = [10,6,4,18]
    print(test1)
    print('required {} time unit'.format(get_requiredtime(test1)))
