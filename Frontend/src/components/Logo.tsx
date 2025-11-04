import logoSvg from "../assets/logo.svg";

export default function Logo({ className = "w-8 h-8" }: { className?: string }) {
    return (
        <img
            src={logoSvg}
            alt="Conexus Billing"
            className={className}
        />
    );
}
