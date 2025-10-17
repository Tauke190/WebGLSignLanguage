# 🧏‍♀️ Sign Language Animator (WebGL)

A WebGL-based **Sign Language Animation System** that brings digital avatars to life with synchronized hand gestures and lip movements.  
This project enables seamless integration with other web applications via `iframe` and supports **lip synchronization** using **Microsoft Azure Viseme API** for real-time audio-to-lip animation.

---

## 🚀 Features

- 🎮 **Interactive Sign Language Animation**  
  Animates avatar gestures for various sign language expressions.

- 🔊 **Lip Sync with Microsoft Azure**  
  Translates audio input into synchronized lip movements using Azure’s **Speech-to-Viseme** mapping.

- 🌐 **WebGL Export and Integration**  
  The project can be built as a WebGL application and embedded into any web page using an `<iframe>`.

- ⚙️ **Modular Scene Design**  
  Different Unity scenes handle specific tasks for modularity and scalability.

---

## 🧩 Unity Scenes Overview

| Scene Name | Description |
|-------------|-------------|
| **GameScene.unity** | The main scene where core sign language animations and gestures are showcased. Acts as the entry point for WebGL builds. |
| **GameSceneMale.unity** | Scene containing the **male avatar** version of the sign language animator. Demonstrates gesture accuracy and motion blending specific to the male character. |
| **GameSceneFemale.unity** | Scene containing the **female avatar** version of the sign language animator. Includes gestures and animation sets customized for the female model. |
| **LipSync.unity** | Scene responsible for **lip synchronization**. Uses Microsoft Azure’s Speech SDK to map real-time audio visemes (phoneme shapes) onto the avatar’s mouth for accurate speech animation. |

---

## 🧠 Technical Overview

- **Engine:** Unity 2022.3.39f1 (LTS)
- **Build Target:** WebGL
- **Lip Sync Provider:** Microsoft Azure Cognitive Services (Viseme API)
- **Integration:** Supports cross-app embedding via `<iframe>`  
- **Languages:** C#, JavaScript (for WebGL embedding)

---

## 🌍 Integration Example

You can embed the WebGL build into any webpage using an iframe:

```html
<iframe 
  src="https://yourdomain.com/SignLanguageAnimator/index.html" 
  width="960" 
  height="600" 
  frameborder="0"
  allow="autoplay; microphone">
</iframe>
