import facial_req
import facial_req2
import headshots
import train_model

if __name__ == "__main__":
    headshotAddition = input('Do you need to add additional people? Please enter 1 for Yes and 2 for No\n')
    headshotAdditionInteger = int(headshotAddition)
    while (headshotAdditionInteger != 1) and (headshotAdditionInteger != 2):
        headshotAdditionInteger = int(input('Sorry that is an invalid response. Please enter 1 for Yes and 2 for No\n'))
    if headshotAddition == '1':
        headshots.headshotCapture()
        train_model.train_model()
        facial_req.facial_recognition()
    else:
        train_model.train_model()
        facial_req2.detect()

