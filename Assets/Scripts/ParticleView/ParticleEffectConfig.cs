using UnityEngine;

[CreateAssetMenu(menuName = "ParticleEffect/Config")]
public class ParticleEffectConfig : ScriptableObject
{
  [SerializeField] private ParticleEffectStyleConfig[] styles;
  public ParticleEffectStyleConfig[] Styles => styles;
}