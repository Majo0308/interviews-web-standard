import { onMounted, ref } from "vue";
import confetti from "canvas-confetti";

export function useConfetti(canvasRef: any) {
  const localConfetti = ref<ReturnType<typeof confetti.create> | null>(null);

  onMounted(() => {
    if (canvasRef.value) {
      localConfetti.value = confetti.create(canvasRef.value, {
        resize: true,
        useWorker: true,
      });
    }
  });

  const fireConfetti = () => {
    if (!localConfetti.value) return;

    const fire = (x: number, y: number) => {
      localConfetti.value?.({
        particleCount: 30,
        spread: 60,
        startVelocity: 30,
        origin: { x, y },
        colors: ["#00cfff", "#00e676", "#ffea00"],
      });
    };
    fire(0.2, 0.2);
    fire(0.8, 0.2);
    fire(0.2, 0.8);
    fire(0.8, 0.8);
    fire(0.5, 0.5);
  };

  return { fireConfetti };
}
