from torch.nn.init import *
import matplotlib.pyplot as plt

from face_recognizer import model
from face_recognizer.model import optimizer_ft, exp_lr_scheduler, criterion
from face_recognizer.train import train_model

model_ft, FT_losses = train_model(model, criterion, optimizer_ft, exp_lr_scheduler, num_epochs=200)
plt.figure(figsize=(10,5))
plt.title("FRT Loss During Training")
plt.plot(FT_losses, label="FT loss")
plt.xlabel("iterations")
plt.ylabel("Loss")
plt.legend()
plt.show()

torch.save(model, "/model.pt")