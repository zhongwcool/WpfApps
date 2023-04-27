# This is a sample Python script.
import random
import sys
import time


# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.


def print_hi(name):
    num = random.randint(1, 99)
    print(f"开始等待{num}s...")
    time.sleep(num)
    print("等待结束")
    # Use a breakpoint in the code line below to debug your script.
    print(f'Hi, {name}')  # Press Ctrl+F8 to toggle the breakpoint.


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    # 输出脚本文件名称
    print("脚本名称：", sys.argv[0])

    # 输出传递的参数
    for i in range(1, len(sys.argv)):
        print("参数 %d: %s" % (i, sys.argv[i]))

    if len(sys.argv) > 1:
        print_hi(sys.argv[1])
    else:
        print_hi('PyCharm')

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
