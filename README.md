# Saving Dodo

## Introduction
My undergraduate work, an independently developed 3D audio game based on the Unity engine (implemented by C#) that has won many awards. The theme of the game is the protection of birds. It's worth mentioning that we collaborated with art students to create the story, characters, etc. It's a complete game, all in all, so feel free to download the EXE to experience it!

## Video demonstration
https://www.bilibili.com/video/BV1pc411j7Nf/?vd_source=fcea5d5c4b86183c43910448bde4113d

<img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig12.png" height="200" />  <img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig13.png" height="200" /><br/>

<img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/图片5.gif" width="718" />



## Project Overview
The key design and technical implementation of the project is as follows：

- Zero-copy implementation of real-time music beat extraction.
    - First, the Fourier transform is used to transform audio files into the frequency domain and to obtain the relative amplitude values of the frequency bin complex data.
    - Second, calculation of the spectral flux (i.e. the sum of the differences between the amplitude values corresponding to the frequency bins of adjacent frames).
    - Third, spectral flux data was trimmed with the aim of finding distinct beats.
    - Finally, trimmed spectral flux of all frames within a sliding window is averaged and used as a threshold for extracting beats.
    - This technology enables support for beat analysis of any user uploaded songs.

<div align=center>
<img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig5.jpg" height="150" />  <img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig6.jpg" height="150" /><br/>
<img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig3.png" height="150" />            <img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig4.png" height="150" />            <img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig7.png" height="150" />
</div>


- Zero-copy implementation of soft object technology.
    - First, get the bounds property of the sprite.
    - Second, create a new mesh grid, which involves initialising the mesh vertices and initialising the materials.
    - Third, generate rigid body combinations. Initialise whether each rigid body is rotated or not Initialise the offset of each rigid body with respect to the central rigid body.
    - Fourth, calculate the weight values for each rigid structure (each structure has a different influence on the vertices, with closer reference points contributing more to the final position than more distant ones).
    - Finally, update the soft object through using position offsets relative to the central rigid body.

<div align=center>
<img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/图片3.gif" height="150" />            <img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/图片4.gif" height="150" /><br/>
</div>


- The parameters related to the difficulty of the game are dynamically adjusted by tracking the user's progress and performance in real time.
<div align=center>
    
<img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig8.jpg" height="150" />            <img src="https://github.com/fwyc0573/SavingDodo/blob/main/fig/fig9.jpg" height="150" /><br/>
</div>


- To achieve better performance, object pool management techniques are introduced.
- Infinite loop generation of the game map by tracking the position of the game characters.



## Copyright
We have applied for a software copyright for this project (including design of artwork characters and props). Please contact me for any commercial use.
