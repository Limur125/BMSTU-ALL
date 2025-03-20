import pandas as pd
import numpy as np
from sklearn.neighbors import NearestNeighbors
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.decomposition import PCA
from sklearn.manifold import TSNE, MDS
from umap import UMAP
from sklearn.cluster import KMeans, DBSCAN
from sklearn.metrics import silhouette_score
from sklearn.pipeline import Pipeline
import matplotlib.pyplot as plt
import seaborn as sns

boardGames = pd.read_csv('bgg_dataset.csv', delimiter=';')
mech = boardGames['Mechanics']
mechs = set()
for m in mech:
    if type(m) == str:
        mSplit = m.split(', ')
        for ms in mSplit:
            mechs.add(ms)
mechsarr = list()
mechsList= list(mechs)
for m in mech:
    mechsarri = np.zeros(shape=len(mechs))
    if type(m) == str:
        mSplit = m.split(', ')
        for ms in mSplit:
            i = mechsList.index(ms)
            mechsarri[i] += 1
    mechsarr.append(mechsarri)
dfMechs = pd.DataFrame(mechsarr, columns=[f'm{i}' for i in range(len(mechs))])
df3 = pd.merge(dfMechs, boardGames, left_index=True, right_index=True)
boardGames = boardGames.drop('Mechanics', axis=1)
df3 = df3.drop('Mechanics', axis=1)

dom = boardGames['Domains']
doms = set()
for d in dom:
    if type(d) == str:
        dSplit = d.split(', ')
        for ds in dSplit:
            doms.add(ds)
domsarr = list()
domsList= list(doms)
for d in dom:
    domsarri = np.zeros(shape=len(doms))
    if type(d) == str:
        dSplit = d.split(', ')
        for ds in dSplit:
            i = domsList.index(ds)
            domsarri[i] += 1
    domsarr.append(domsarri)
dfdoms = pd.DataFrame(domsarr, columns=[f'{domsList[i]}' for i in range(len(doms))])
df4 = pd.merge(dfdoms, df3, left_index=True, right_index=True)
boardGames = pd.merge(dfdoms, boardGames, left_index=True, right_index=True)
boardGames = boardGames.drop('Domains', axis=1)
df4 = df4.drop('Domains', axis=1)
print(boardGames)
#plt.figure(figsize= (15,12))
#sns.heatmap(boardGames.corr().round(decimals=2), annot=True)
#plt.show()

scaler = StandardScaler()
data_scaled = scaler.fit_transform(boardGames)

pca = PCA(n_components=2)
tsne = TSNE(n_components=2)
umap = UMAP(n_components=2)

pca_data = pca.fit_transform(data_scaled)
tsne_data = tsne.fit_transform(data_scaled)
umap_data = umap.fit_transform(data_scaled)


def plot_clusters(X, title, ax, proj3d=False):
    for p in X:
        ax.scatter(p[0], p[1])
    ax.set_title(title)


def plot3d(data, ax):
    for i in range(0, len(data), 3):
        ax.scatter(data[i][0], data[i][1], data[i][2])

fig, axes = plt.subplots(2, 2, figsize=(15, 22))
fig.suptitle('Результаты', fontsize=16)
plot_clusters(pca_data, 'PCA', axes[0, 0])
plot_clusters(tsne_data, 't-SNE', axes[1, 0])
plot_clusters(umap_data, 'UMAP', axes[0, 1])
plot_clusters(umap_data, 'UMAP', axes[0, 1])
plt.show()

# fig = plt.figure()
# ax = fig.add_subplot(projection ='3d')
# plot3d(tsne_data, ax)
# plt.show()



